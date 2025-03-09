export const base_url :string = "https://localhost:7032";
export const image_actions  :string = "/api/BMP";
export const sound_actions  :string = "/api/WAV";
export const model_actions  :string = "/api/GLB";
export const user_actions :string = "/api/Auth";
export const pref_actions :string = "/api/Preferences";

export const log_in ="/LogIn"

export let http_timeout:number =0;

export function set_http_timeout(time:number){
 
    http_timeout=Math.max(0,(Math.round(time/1000)-900000));
}

export function force_reload(){

    localStorage.removeItem('authToken');
    sessionStorage.clear();
    window.location.reload();


}


