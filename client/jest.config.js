module.exports = {
  errorOnDeprecated: true,
  maxWorkers: '100%',
  setupFilesAfterEnv: ['<rootDir>/setupTests.js'],
  testEnvironment: 'jsdom',
  testMatch: ['**/?(*.)+(spec|test).[tj]s?(x)'],
  moduleNameMapper: {
    "\\.(css|less|scss|sass)$": "identity-obj-proxy"
  },
  resetMocks: true,
  collectCoverage: true,
  collectCoverageFrom: [
    '<rootDir>/src/**/*.{ts,tsx}',
    '!<rootDir>/src/index.tsx', // Entrypoint
    '!<rootDir>/src/service-worker.ts', // Entrypoint
    '!<rootDir>/src/test-utils.tsx', // Test utilities, not production code
    '!<rootDir>/src/infra/RpcClient.ts', // Generated code
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
