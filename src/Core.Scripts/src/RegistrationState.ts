export type RegistrationState = {
  definedKeys: Set<string>;
};

export function getRegistrationState(): RegistrationState {
  const scope = globalThis as typeof globalThis & {
    __fluentUIBlazorDefinedKeys__?: RegistrationState;
  };

  if (!scope.__fluentUIBlazorDefinedKeys__) {
    scope.__fluentUIBlazorDefinedKeys__ = {
      definedKeys: new Set<string>()
    };
  }

  return scope.__fluentUIBlazorDefinedKeys__;
}

export function defineOnce(key: string, callback: () => void) {
  const state = getRegistrationState();

  if (state.definedKeys.has(key)) {
    return;
  }

  callback();
  state.definedKeys.add(key);
}
