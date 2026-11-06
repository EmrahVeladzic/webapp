export const base_url :string = "https://localhost:7032";
export const image_actions  :string = "/api/BMP";
export const sound_actions  :string = "/api/WAV";
export const model_actions  :string = "/api/GLB";
export const export_actions : string="/api/Export";

export function force_reload(){

    sessionStorage.clear();
    window.location.reload();

}