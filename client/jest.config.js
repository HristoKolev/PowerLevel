module.exports = {
  errorOnDeprecated: true,
  maxWorkers: '100%',
  setupFilesAfterEnv: ['<rootDir>/setupTests.js'],
  testEnvironment: 'jsdom',
  testMatch: ['**/?(*.)+(spec|test).[tj]s?(x)'],
  collectCoverage: true,
  collectCoverageFrom: [
    '<rootDir>/src/**/*.{ts,tsx}',
    '!<rootDir>/src/index.tsx', // Entrypoint
    '!<rootDir>/src/service-worker.ts', // Entrypoint
    '!<rootDir>/src/infrastructure/test-utils.tsx', // Test utilities, not production code
    '!<rootDir>/src/rpc/RpcClient.ts', // Generated code
  ],
  coverageDirectory: 'coverage',
  coverageThreshold: {
    global: {
      lines: 90,
      statements: 90,
      functions: 90,
    },
  },
};
