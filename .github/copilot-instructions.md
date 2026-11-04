# Copilot Instructions

## Project Guidelines
- User intends to adopt a Redux pattern: GameState and its properties should be immutable/readonly.
- System.Linq usings are unnecessary; solution builds without them.

## Rendering Guidelines
- Prefer ANSI truecolor SGR escapes over ConsoleColor for this renderer.
- Use standard RGB for red/green/blue, a midpoint yellow between DarkYellow and Yellow, true brown, and white for Polymath.

## Player Tableau Display Guidelines
- Omit hyphens and costs from the display.
- Color only the card name and keep the panel border uncolored; ensure the entire name remains consistently colored, including across wrapping.
- Color card names according to their bonus color.
- Display victory points only when the value is at least one.
- For player tableau summaries, show counts only (no labels) by bonus color; use each bonus color as the count background with white foreground, use dark-gray foreground for polymath, gray the passive player's count foreground and darken that player's tableau border, and show summed tableau-plus-leader victory points right-aligned beside the player name.