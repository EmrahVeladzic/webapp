export function get_bits_per_index(clut_count:number):number{

    if(clut_count>16){
        return 8;
    }
    else{
        return 4;
    }

}

export function get_bit_mask(bpi:number):number{

    if(bpi==1){
        return 0x01;
    }

    else if (bpi==2){

        return 0x03;
    }

    else if (bpi==4){

        return 0x0F;
    }

    else{

        return 0xFF;
    }

}

export function get_pcm_value(nibble:number, shift:number, filter: number, old: number, older:number):number{

    let mult = (1<<(12-shift));

    let val = nibble;

    if(val>7){
        val-=16;
    }

    val*=mult;  

    switch(filter){        
        case 1: val+=((60*old)+32)/64; break;
        case 2: val+=((115*old)-(52*older)+32)/64; break;
        case 3: val+=((98*old)-(55*older)+32)/64; break;
        case 4: val+=((122*old)-(60*older)+32)/64; break;
        default: break;
    }

    if (val > 32767){ 
    val = 32767;}
    else if (val < -32768){
    val = -32768;}


    return val;

}