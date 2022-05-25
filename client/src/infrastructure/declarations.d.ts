declare module '*.svg' {
  const url: string;
  export default url;
}

interface Window {
  __browserSupported: boolean;
}
