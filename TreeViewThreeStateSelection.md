# Rapport de revue : sélection à trois états du TreeView

## Périmètre

- Branche analysée : `users/dvoituron/test-treeview-indeterminate`
- Révision analysée : `92105e3095cc9f29059be5af48af537c1096a4cf`
- Branche de référence : `dev`
- Révision de référence : `5f7bda4052c612ff4efcaecbe1eac7a0e1f92c90`
- Date : 9 octobre 2026
- Méthode : revue complète suivant le protocole `code-review-orchestration.md`, avec quatre pistes indépendantes.
- Aucun code modifié et aucune publication sur GitHub dans le cadre de la revue.

## Conclusion

L'approche JavaScript est globalement saine : la branche effectue **un seul appel JS batché par arbre**, et non un appel par checkbox. Pour des arbres petits ou moyens, le coût restera probablement acceptable.

En revanche, deux points méritent une correction avant de considérer l'API comme optimisée :

1. `UpdateCheckStates` est appelé après **chaque rendu** lorsque `CheckState` est défini, même si aucun état n'a changé.
2. `TreeViewThreeStateSelection.GetCheckState` peut entraîner un coût quadratique et de nombreuses allocations. Sur les grands arbres, ce calcul .NET risque de coûter davantage que l'appel JavaScript lui-même.

Les estimations de complexité ci-dessous résultent de l'analyse du code ; aucun benchmark de latence ou d'allocation n'a été réalisé.

## Résultats principaux

### 1. Priorité haute : calcul potentiellement quadratique des états

**Corrigé dans le code local le 9 octobre 2026.** Le constat ci-dessous décrit la révision
analysée avant correction. Le helper utilise désormais un seul dictionnaire pour stocker
la sélection (`true`) et mémoriser les états calculés des nœuds (`false` ou `null`).
Pour un arbre, l'affectation de la sélection puis l'évaluation de tous les états
coûte au total `O(N + S)` entre deux invalidations, puis les lectures en cache coûtent `O(1)`.
Toute affectation de `Items` ou `SelectedItems`, y compris avec la même référence, invalide
le cache ; `OnSelectedItemsChanged` le fait via son affectation de `SelectedItems`.
L'affectation de `SelectedItems` copie la sélection fournie : les mutations ultérieures
d'une liste externe nécessitent une nouvelle affectation. Après une mutation directe de
l'arbre ou des descendants, appeler `Refresh()` avant le prochain rendu. Cette méthode
conserve les éléments sélectionnés, supprime les états calculés et ne déclenche pas de rendu.
Le getter `SelectedItems` filtre le dictionnaire sans conserver une seconde collection ;
son énumération est linéaire dans le nombre d'entrées stockées.

Validation de la première correction : **62 tests ciblés réussis**, dont 12 nouveaux cas couvrant
les invalidations et les énumérations sur des arbres de 32 et 256 nœuds.
Validation de la simplification à un dictionnaire unique : **70 tests ciblés réussis**.
Les huit cas supplémentaires couvrent notamment la copie immédiate de la sélection,
les doublons, la réaffectation depuis une énumération du getter, les erreurs d'énumération
et la conservation de la sélection lors des rafraîchissements.

```powershell
dotnet test --project tests\Core\Components.Tests.csproj --no-restore --filter-class "*TreeViewThreeStateSelectionTests" --filter-class "*FluentTreeCheckStateTests"
```

Dans [`GetCheckState`](src/Core/Components/TreeView/TreeViewThreeStateSelection.cs), chaque appel :

- reconstruit un `HashSet` contenant toute la sélection ;
- parcourt récursivement les descendants de l'élément si celui-ci n'est pas sélectionné.

Or cette méthode est appelée une fois par checkbox pendant le rendu depuis [`AddFluentTreeItemChildContent`](src/Core/Components/TreeView/FluentTreeItem.razor.cs).

Selon la forme de l'arbre et la taille de la sélection, un rendu peut donc coûter approximativement :

- `O(N × S)` uniquement pour reconstruire les ensembles ;
- jusqu'à `O(N²)` pour les parcours répétés des sous-arbres ;
- avec jusqu'à `N` allocations de `HashSet`.

Ici, `N` désigne le nombre de nœuds évalués pendant le rendu et `S` la taille de la sélection. Le pire cas récursif suppose notamment un arbre profond et des états qui ne permettent pas de terminer rapidement la recherche.

Dans la démo de 225 éléments, un rendu évaluant toutes les checkboxes représente déjà 225 créations d'ensembles.

**Recommandation**

Calculer une seule fois, lorsque `Items` ou `SelectedItems` change :

- un `HashSet<ITreeViewItem>` de la sélection ;
- idéalement un dictionnaire associant chaque `ITreeViewItem` à son état `bool?`, calculé par un parcours post-order de l'arbre.

`GetCheckState` deviendrait alors une simple lecture en `O(1)`. Cette optimisation est plus importante que de micro-optimiser la boucle TypeScript.

L'invalidation doit tenir compte des mutations des collections et des descendants, pas uniquement du remplacement de leurs références.

### 2. Priorité moyenne : appel interop JS après chaque rendu

Dans [`OnAfterRenderAsync`](src/Core/Components/TreeView/FluentTreeView.razor.cs), en sélection multiple et tant que `CheckState` est défini, chaque rendu exécute :

```text
.NET → JavaScript → UpdateCheckStates
```

Au premier rendu, deux appels de fonction successifs sont effectués, en plus de l'import éventuel du module :

1. `Initialize`
2. `UpdateCheckStates`

Puis chaque rendu ultérieur déclenche un nouvel appel à `UpdateCheckStates`, y compris lorsque le rendu provient d'un changement sans rapport avec les checkboxes.

Conséquences :

- en WebAssembly : sérialisation et passage de frontière .NET/JS ;
- en Blazor Server : échange SignalR supplémentaire ;
- côté navigateur : nouvelle recherche DOM complète.

La fonction [`UpdateCheckStates`](src/Core/Components/TreeView/FluentTreeView.razor.ts) effectue ensuite :

- un `querySelectorAll` sur toutes les checkboxes correspondantes ;
- un `closest("fluent-tree")` pour chacune ;
- deux écritures de propriétés pour chaque checkbox contrôlée ou précédemment contrôlée.

Les parcours d'ancêtres peuvent coûter jusqu'à `O(C × profondeur)` par appel, en plus de la recherche DOM, où `C` est le nombre de checkboxes correspondantes dans le sous-arbre interrogé.

#### Points positifs

L'implémentation évite plusieurs écueils importants :

- un seul appel interop est effectué pour tout l'arbre ;
- aucun appel JS par checkbox ;
- les checkboxes appartenant aux arbres imbriqués ne sont pas modifiées ;
- le `WeakSet` ne retient pas les éléments supprimés du DOM ;
- aucune boucle de rendu .NET/JS n'a été identifiée ;
- un passage de nettoyage est prévu après suppression de `CheckState` en sélection multiple pour restaurer les propriétés vivantes.

#### Optimisation recommandée

Ajouter une notion d'état « dirty » :

- comparer les nouveaux états `checked`/`indeterminate` avec ceux du rendu précédent ;
- appeler `UpdateCheckStates` uniquement lorsqu'une synchronisation est nécessaire ;
- forcer le passage lorsque :
  - `CheckState` est ajouté ou supprimé ;
  - une checkbox contrôlée reçoit une interaction utilisateur ;
  - la visibilité ou la liste des éléments rendus change.

Il faut conserver le batch global : remplacer cet appel par plusieurs petits appels JS serait une régression.

Une alternative serait un `MutationObserver` installé dans `Initialize`, mais cela complexifierait davantage le cycle de vie et devrait aussi traiter explicitement les interactions utilisateur qui modifient uniquement la propriété vivante. Le dirty flag côté Blazor paraît plus simple.

### 3. Priorité moyenne : `CheckState` est rappelé hors du rendu

La documentation de [`CheckState`](src/Core/Components/TreeView/FluentTreeView.razor.cs) indique :

> The function is evaluated during rendering.

Pourtant, [`OnCheckChangedHandlerAsync`](src/Core/Components/TreeView/FluentTreeItem.razor.cs) l'appelle de nouveau au clic via :

```csharp
OwnerTreeView.GetCheckState(checkedItem)
```

Si le callback dépend d'un état externe ayant changé depuis le rendu, ou s'il est non déterministe, l'action peut inverser un état différent de celui réellement affiché.

**Recommandation**

Capturer `checkState` lors de la construction de la checkbox et transmettre cette valeur au gestionnaire. Le clic agirait alors sur l'état affiché, sans réévaluer le callback en dehors du rendu.

## Point non retenu comme régression

Le fallback :

```csharp
SelectedItems?.Contains(item)
```

peut coûter `O(N × S)` lorsque `SelectedItems` n'est pas une collection à recherche rapide. Cependant, ce comportement existait déjà dans `dev`. Il ne constitue donc pas une régression de cette branche, même si une matérialisation unique en `HashSet` serait bénéfique à terme.

## Validation rapportée par la piste de preuve

- `dotnet test tests\Core\Components.Tests.csproj --filter "FullyQualifiedName~TreeViewThreeStateSelectionTests|FullyQualifiedName~FluentTreeCheckStateTests" --no-restore`
  - **50 tests réussis, 0 échec**
- `dotnet build src\Core\Microsoft.FluentUI.AspNetCore.Components.csproj --no-restore`
  - **Succès, 0 avertissement, 0 erreur**
- `git diff --check dev...HEAD`
  - **Aucune anomalie**
- Démo lancée sur `http://localhost:5099/TreeView`.
- Vérification interactive rapportée :
  - 225 checkboxes contrôlées initialement synchronisées ;
  - clic sur une racine ;
  - 45 descendants passés à l'état coché ;
  - attributs et propriétés DOM concordants.

### Limites de la preuve

- Une compilation normale de la démo a été bloquée par un fichier `FluentUI.Demo.SampleData.dll` verrouillé ; la démo a été lancée avec `--no-build`. Cette observation navigateur ne garantit donc pas, à elle seule, l'identité des binaires exécutés avec la révision analysée.
- La transition indéterminée n'a pas été exercée visuellement.
- La suppression dynamique de `CheckState` n'a été couverte que par les tests de rendu, pas par une interaction navigateur.
- Le chemin navigateur n'a pas fourni d'artefact persistant exploitable dans ce rapport.
- La suite complète n'a pas été exécutée.
- Aucun benchmark comparatif avec `dev`, aucune mesure de latence interop et aucune mesure d'allocations n'ont été réalisés.

## Avis final

| Aspect | Avis |
|---|---|
| Batch JS | Bon : un appel par arbre, pas par checkbox |
| Fréquence d'interop | Trop élevée : un appel après chaque rendu contrôlé |
| Parcours DOM | Acceptable ponctuellement, coûteux s'il est répété inutilement |
| `WeakSet` | Approprié et sans fuite évidente |
| Calcul tri-state .NET | Principal risque de performance |
| Correction fonctionnelle | Bonne globalement, avec un risque de réévaluation au clic |
| Tests | Bonne couverture unitaire, preuve navigateur partielle |

Je ne bloquerais pas la branche uniquement à cause de `UpdateCheckStates` pour des arbres de taille modérée. En revanche, avant de publier `TreeViewThreeStateSelection` comme helper public destiné aux grands arbres, je corrigerais son recalcul quadratique. J'ajouterais ensuite un mécanisme dirty pour éviter les appels JS sans changement réel.

## Pistes de revue utilisées

La revue complète a été retenue en raison de l'ajout d'une API publique et du risque de régression UX interactive. Les conventions C#/Blazor du dépôt ont été utilisées.

| Piste | Modèle/effort | Résultat |
|---|---|---|
| Architecture et simplicité | Géré par le runtime, détails indisponibles | Aucun problème bloquant |
| Régressions et cas limites | Géré par le runtime, détails indisponibles | Réévaluation de `CheckState` au clic |
| Domaine et performances | Géré par le runtime, détails indisponibles | Calcul quadratique et interop répétée |
| Preuve d'exécution | Géré par le runtime, détails indisponibles | Build et 50 tests réussis, démo exercée avec les limites ci-dessus |
