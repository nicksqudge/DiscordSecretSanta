// @ts-ignore
declare global {
  namespace Cypress {
    interface Chainable {
      hasComponent(componentSelector: string): void;
    }
  }
}
