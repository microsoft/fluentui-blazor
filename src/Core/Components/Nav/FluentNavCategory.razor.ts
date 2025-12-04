export namespace Microsoft.FluentUI.Blazor.NavDrawer {

export function ToggleCategory(categoryId: string, useSingleExpand: boolean): void {
    const category = document.getElementById(categoryId) as HTMLButtonElement;
    if (!category) return;

    const group = category.nextElementSibling as HTMLElement;
    if (!group || !group.classList.contains('fluent-navsubitemgroup')) return;

    const isExpanded = group.classList.contains('expanded');

    if (useSingleExpand && !isExpanded) {
        collapseAllExcept(group);
    }

    if (isExpanded) {
        collapse(group, category);
    } else {
        expand(group, category);
    }
}



export function ExpandCategory(categoryId: string, useSingleExpand: boolean): void {
    const category = document.getElementById(categoryId) as HTMLElement;
    if (!category) return;

    const group = category.nextElementSibling as HTMLElement;
    if (!group || group.classList.contains('expanded')) return;

    if (useSingleExpand) {
        collapseAllExcept(group);
    }

    expand(group, category);
}

export function CollapseCategory(categoryId: string): void {
    const category = document.getElementById(categoryId) as HTMLElement;
    if (!category) return;

    const group = category.nextElementSibling as HTMLElement;
    if (group && group.classList.contains('expanded')) {
        collapse(group, category);
    }
}

export function CollapseAllCategories(): void {
    const allGroups = document.querySelectorAll<HTMLElement>('.fluent-navsubitemgroup.expanded');
    allGroups.forEach(group => {
        const category = group.previousElementSibling as HTMLElement;
        collapse(group, category);
    });
}

// Animation helpers
function expand(element: HTMLElement, category: HTMLElement): void {
    element.classList.add('expanded');
    category.setAttribute('aria-expanded', 'true');

    const height = element.scrollHeight;
    element.style.transition = 'max-height 0.3s ease, opacity 0.3s ease';
    element.style.maxHeight = '0px';
    element.style.opacity = '0';

    requestAnimationFrame(() => {
        element.style.maxHeight = `${height}px`;
        element.style.opacity = '1';
    });

    element.addEventListener('transitionend', () => {
        element.style.maxHeight = 'none';
    }, { once: true });
}

function collapse(element: HTMLElement, category: HTMLElement): void {
    const height = element.scrollHeight;
    category.setAttribute('aria-expanded', 'false');

    element.style.transition = 'max-height 0.3s ease, opacity 0.3s ease';
    element.style.maxHeight = `${height}px`;
    element.style.opacity = '1';

    requestAnimationFrame(() => {
        element.style.maxHeight = '0px';
        element.style.opacity = '0';
    });

    element.addEventListener('transitionend', () => {
        element.classList.remove('expanded');
    }, { once: true });
}

function collapseAllExcept(exceptElement: HTMLElement): void {
    const allGroups = document.querySelectorAll<HTMLElement>('.fluent-navsubitemgroup.expanded');
    allGroups.forEach(group => {
        if (group !== exceptElement) {
            const category = group.previousElementSibling as HTMLElement;
            collapse(group, category);
        }
    });
}

}
