export function toggle_visibility($id:string, $visible : boolean , $resize : boolean){

    var $element = document.getElementById($id) as HTMLDivElement;

    if($visible){
        $element!.style.visibility="visible";
        if($resize){
            $element!.style.height="100%";
        }
       
    }
    else{
        $element!.style.visibility="collapse";
        if($resize){
            $element!.style.height="0%";
        }
        
    }
      
}

export function reset_visibility($ids:string[]){

    $ids.forEach($id => {
        
        toggle_visibility($id,false,true);

    });

}