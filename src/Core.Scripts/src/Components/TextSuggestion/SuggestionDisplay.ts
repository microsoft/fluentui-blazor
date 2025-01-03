export interface SuggestionDisplay {
  show(suggestion: string): void;
  accept(): void;
  reject(): void;
  isShowing(): boolean;

  get currentSuggestion(): string;
}
