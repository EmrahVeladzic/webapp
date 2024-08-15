export async function hash_data(input:string):Promise<string>{

    let data_bfr = new TextEncoder().encode(input);
    let hash_bfr = await crypto.subtle.digest('SHA-256', data_bfr);
    let out = btoa(String.fromCharCode(...new Uint8Array(hash_bfr)));
    
    return out;
}