export function get_interpolation_value(current:number, former:number,next:number):number{

    while(next<former){
        next+=former;
    }

    const range = next - former;
    const value = ( (current - former) / range ) ;    

    return Math.min(Math.max(value, 0.0), 1.0);

return 0;
}