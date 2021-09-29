importScripts("lib/workbox-v6.2.4/workbox-sw.js");
importScripts("fetchHandler.js");

self.addEventListener("message", ({ data }) => {
    if (data === "skipWaiting") {
        self.skipWaiting();
    }
});

const match = ({url, event}) => {
    if (url.pathname === '/example') {
        return {
            name: 'Workbox',
            type: 'guide',
        };
    }
};

const handler = async ({url, event}) => {
    return new Response(`Custom handler response.`);
};


self.addEventListener('push', event => {
    console.log(`Push received with data "${event.data.text()}"`);

    const title = 'Hakuna matata.';
    const options = {
        body: `${event.data.text()}`,
        data: { href: '/users/donald' },
        actions: [
            { action: 'details', title: 'Details' },
            { action: 'dismiss', title: 'Dismiss' },
        ],
    };

    event.waitUntil(self.registration.showNotification(title, options));
});
const handlerJwt = async ({url, fetchEvent}) => {
    const request = fetchEvent.request;
  
    if(url.pathname === '/Account/Login') {
        console.debug('Bypass sw on api auth');
        fetchEvent.respondWith(fetch(request));
        return;
    }

    if(request.mode === 'navigate' && request.method !== 'GET') {
        console.log('Bypass sw on post navigation');
        fetchEvent.respondWith(fetchEvent(request));
        return;
    }

    fetchEvent.respondWith(self.getResponse(request));

};

const getResponse = async request => {
    const headers = {};
    for (let entry of request.headers) {
        headers[entry[0]] = headers[entry[1]];
    }

    const token =localStorage.getItem("id-token") //await getAuthTokenHeader();
    // if(token === null) {
    //     if(request.mode === 'navigate') {
    //         return new Response(null, {
    //             status: 302,
    //             statusText: 'Found',
    //             headers: new Headers({
    //                 'location': '/login',
    //             })
    //         })
    //     }
    //
    //     return new Response(null, {
    //         status: 401,
    //         statusText: 'Unauthorized'
    //     });
    // }

    headers['Authorization'] = `Bearer ${token}`;
    const body = await ['HEAD', 'GET'].includes(request.method) ? Promise.resolve() : request.text();
    const bodyObj = body ? {body,}: {};

    return fetch(new Request(request.url, {
        method: request.method,
        headers,
        cache: request.cache,
        mode: 'cors',
        credentials: request.credentials,
        redirect: 'manual',
        ...bodyObj,
    }));
};

self.workbox.routing.registerRoute("/api/oauth/dialog", handler);
self.workbox.routing.registerRoute( ({ url }) => {
    return  true;
}, handlerJwt);
// self.workbox.precaching.precacheAndRoute([self.__WB_MANIFEST]);
