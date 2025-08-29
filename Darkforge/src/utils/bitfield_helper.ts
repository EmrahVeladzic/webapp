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

export function get_pcm_value(nibble:number, combined_shift:number):number{

    let mult = (1<<combined_shift);

    let val = nibble;

    if(val>7){
        val-=16;
    }

    val*=mult;  

    return (val/32768);

}