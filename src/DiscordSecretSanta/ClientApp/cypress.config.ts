import { defineConfig } from "cypress";

export default defineConfig({
  e2e: {
    baseUrl: 'https://localhost:44415/',
    setupNodeEvents(on, config) {
      // implement node event listeners here
    },
  },
});
