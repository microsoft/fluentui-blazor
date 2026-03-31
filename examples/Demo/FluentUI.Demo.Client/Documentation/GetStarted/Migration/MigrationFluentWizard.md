---
title: Migration FluentWizard
route: /Migration/Wizard
hidden: true
---

- ### Component removed 💥

  `FluentWizard` and `FluentWizardStep` have been **removed** in V5.
  There is no direct replacement component.

- ### V4 FluentWizard parameters (removed)

  | Parameter | Type | Default |
  |-----------|------|---------|
  | `Height` | `string` | `"400px"` |
  | `Width` | `string` | `"100%"` |
  | `OnFinish` | `EventCallback` | — |
  | `StepperPosition` | `StepperPosition` | `Left` |
  | `StepperSize` | `string?` | — |
  | `StepperBulletSpace` | `string?` | — |
  | `Border` | `WizardBorder` | `None` |
  | `DisplayStepNumber` | `WizardStepStatus` | `None` |
  | `Value` / `ValueChanged` | `int` / `EventCallback<int>` | `0` |
  | `ButtonTemplate` | `RenderFragment<int>?` | — |
  | `Steps` | `RenderFragment?` | — |
  | `StepTitleHiddenWhen` | `GridItemHidden?` | `XsAndDown` |
  | `StepSequence` | `WizardStepSequence` | `Linear` |

- ### V4 FluentWizardStep parameters (removed)

  | Parameter | Type | Default |
  |-----------|------|---------|
  | `Label` | `string` | `""` |
  | `Summary` | `string` | `""` |
  | `Disabled` | `bool` | `false` |
  | `DeferredLoading` | `bool` | `false` |
  | `OnChange` | `EventCallback<FluentWizardStepChangeEventArgs>` | — |
  | `IconPrevious` / `IconCurrent` / `IconNext` | `Icon` | — |
  | `StepTemplate` | `RenderFragment<FluentWizardStepArgs>?` | — |

- ### Removed enums
  - `WizardBorder`
  - `WizardStepSequence`
  - `WizardStepStatus`
  - `StepperPosition`

- ### Migration strategy

  Build a custom wizard using `FluentTabs` for step navigation,
  or implement step-based logic with conditional rendering:

  ```xml
  <!-- V5: Custom wizard pattern -->
  <FluentStack Orientation="Orientation.Vertical">
      <!-- Step indicators -->
      <FluentStack Orientation="Orientation.Horizontal" HorizontalGap="8px">
          @for (int i = 0; i < steps.Length; i++)
          {
              <FluentBadge Appearance="@(i == currentStep ? BadgeAppearance.Filled : BadgeAppearance.Outline)"
                           Color="@(i < currentStep ? BadgeColor.Success : BadgeColor.Informative)">
                  @(i + 1). @steps[i]
              </FluentBadge>
          }
      </FluentStack>

      <!-- Step content -->
      @switch (currentStep)
      {
          case 0: <Step1Content /> break;
          case 1: <Step2Content /> break;
          case 2: <Step3Content /> break;
      }

      <!-- Navigation buttons -->
      <FluentStack Orientation="Orientation.Horizontal" HorizontalGap="8px">
          <FluentButton OnClick="@Previous" Disabled="@(currentStep == 0)">Previous</FluentButton>
          <FluentButton OnClick="@Next" Appearance="ButtonAppearance.Primary">
              @(currentStep == steps.Length - 1 ? "Finish" : "Next")
          </FluentButton>
      </FluentStack>
  </FluentStack>
  ```
