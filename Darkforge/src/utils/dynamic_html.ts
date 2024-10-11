export function toggle_visibility($id:string, $visible : boolean){

    var $element = document.getElementById($id) as HTMLDivElement;

    if($visible){
        $element!.style.visibility="visible";
    }
    else{
        $element!.style.visibility="collapse";
    }
      
}