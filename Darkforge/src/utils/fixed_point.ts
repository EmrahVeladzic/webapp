export function get_fixed(value :number, precision:number=12):number{

return(value/(1<<precision));

}

export function get_float(value:number, precision:number=12):number{
return(value/(1<<precision));
}


export function normalize_uv(Input: number[], width:number, height:number):void{

    for(let i = 0; i < Input.length; i+=2){
       
        Input[i]*=(width/(width-1));
        Input[i+1]*=(height/(height-1));

        Input[i]/=width;
        Input[i+1]/=height;
       
    }

}


export function time_float(Input:number[], FPS:number):void{
    
    for(let i = 0; i< Input.length; i++){
        Input[i]/=FPS;

        if(i>0){
            while(Input[i]<Input[i-1]){
                Input[i]+=1;
            }
        }
    }

}