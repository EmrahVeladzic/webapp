
export const base_url :string = "https://localhost:7032";
export const image_actions  :string = "/api/BMP";
export const sound_actions  :string = "/api/WAV";
export const model_actions  :string = "/api/GLB";
export const user_actions :string = "/api/Auth";
export const pref_actions :string = "/api/Preferences";
export const export_actions : string="/api/Export";

export const log_in ="/LogIn"

export let http_timeout:number =0;

export function set_http_timeout(exp:number){
 
    http_timeout=exp;

}

export function token_valid():boolean{

    const now = Math.floor(Date.now() / 1000);

    if(http_timeout>0 && http_timeout-now<900){

        return false;

    }

    return true;

}

export function force_reload(){

    localStorage.removeItem('DarkforgeAuthToken');
    sessionStorage.clear();
    window.location.reload();


}


