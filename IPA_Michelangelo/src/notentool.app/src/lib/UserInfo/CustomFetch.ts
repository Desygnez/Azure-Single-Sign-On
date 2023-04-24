export async function CustomFetch(url: string): Promise<any> {
    const sessionItem: any = sessionStorage.getItem('aadToken');
    const secret = JSON.parse(sessionItem).accessToken;

    console.log(secret)

    const resp = await fetch(`${url}`, {
        credentials: "include",
        mode: "cors",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            "Authorization": `Bearer ${secret}`
        },
    });
    if (!resp.ok) {
        return Promise.reject(resp);
    }
    return await resp.json();
}


export async function CustomFetchBody(url: string, body: any, method: string): Promise<any> {
    var sessionItem: any = sessionStorage.getItem('aadToken');
    var secret = JSON.parse(sessionItem).accessToken;

    const resp = await fetch(`${url}`, {
        method: method,
        credentials: "include",
        mode: "cors",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            "Authorization": `Bearer ${secret}`
        },
        body: JSON.stringify(body)
    });

    if (!resp.ok) {
        return Promise.reject(resp);
    }
    return await resp.json();

}