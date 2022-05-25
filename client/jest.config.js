module.exports = {
  collectCoverage: true,
  coverageDirectory: 'coverage',
  coverageThreshold: {
    global: {
      lines: 90,
      statements: 90,
    },
  },
  errorOnDeprecated: true,
  maxWorkers: '100%',
  setupFilesAfterEnv: ['<rootDir>/setupTests.js'],
  testEnvironment: 'jsdom',
  testMatch: ['**/?(*.)+(spec|test).[tj]s?(x)'],
  moduleNameMapper: {
    '^~(.*)$': '<rootDir>/src/$1',
  },
};
