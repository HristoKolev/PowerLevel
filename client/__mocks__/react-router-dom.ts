const navigateMock = jest.fn();

// eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
module.exports = {
  ...jest.requireActual('react-router-dom'),
  useNavigate: () => navigateMock,
};
