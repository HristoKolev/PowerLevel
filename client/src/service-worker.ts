// Some strict TS checks are disabled here because we can't set both DOM and WebWorker libraries in tsconfig.json.

/* eslint-disable @typescript-eslint/no-unsafe-assignment,@typescript-eslint/no-unsafe-call,@typescript-eslint/no-unsafe-member-access,@typescript-eslint/no-unsafe-argument,@typescript-eslint/no-explicit-any */

self.addEventListener('fetch', (event) => {
  const fetchEvent = event as any;
  fetchEvent.respondWith(fetch(fetchEvent.request));
});

/* eslint-enable @typescript-eslint/no-unsafe-assignment,@typescript-eslint/no-unsafe-call,@typescript-eslint/no-unsafe-member-access,@typescript-eslint/no-unsafe-argument,@typescript-eslint/no-explicit-any */
