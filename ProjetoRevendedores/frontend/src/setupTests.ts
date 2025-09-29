import '@testing-library/jest-dom/vitest';

// Garante que o objeto navigator esteja definido durante os testes.
Object.defineProperty(globalThis, 'navigator', {
  value: globalThis.navigator ?? ({ onLine: true } as Navigator),
  writable: true
});
