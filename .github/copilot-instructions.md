# Copilot Instructions

## Project Guidelines
- User intends to adopt a Redux pattern: GameState and its properties should be immutable/readonly.
- System.Linq usings are unnecessary; solution builds without them.
- User specifically does not want the XSD schema/template block in the .resx files; preserve resource entries but remove embedded XSD when asked.

## Localization Guidelines
- Use genuine standard .resx files and generated/ResourceManager-backed accessors; do not replace them with hand-written string constant classes or placeholder XSD/resource templates.
- Be candid when something cannot be verified, and do not claim a build passed unless it ran.