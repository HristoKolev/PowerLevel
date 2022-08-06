module.exports = {
  errorOnDeprecated: true,
  maxWorkers: '100%',
  setupFilesAfterEnv: ['<rootDir>/setupTests.js'],
  testEnvironment: 'jsdom',
  testMatch: ['**/?(*.)+(spec|test).[tj]s?(x)'],
  collectCoverage: true,
  collectCoverageFrom: [
    '<rootDir>/src/**/*.{ts,tsx}',
    '!<rootDir>/src/index.tsx',
    '!<rootDir>/src/service-worker.ts',
    '!<rootDir>/src/infrastructure/test-utils.tsx',
    '!<rootDir>/src/rpc/RpcClient.ts',
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
