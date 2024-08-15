import { get_bit_mask, get_bits_per_index } from "../../utils/bitfield_helper";
import { flip_tex_state } from "../../assets/global_assets";

export class Texture{

    public CLUT:number[];
    public Indices:number[];
    public Width : number;
    public Height: number;
    public Data : Uint16Array;

    constructor(clut:number[],pixels:number[],width:number,height:number){

        this.CLUT=clut;
        this.Indices=pixels;
        this.Width=width;
        this.Height=height;

        var bpi = get_bits_per_index(this.CLUT.length);
        var data_length_mult = (8/bpi);

        var mask = get_bit_mask(bpi);

        this.Data = new Uint16Array(this.Width * this.Height);
      
        for(var i =0; i<this.Indices.length;i++){

            var base_byte = this.Indices[i];

            for(var j = 0; j<data_length_mult; j++){
                
               this.Data[(i*data_length_mult)+j]=this.CLUT[((base_byte>>(j*bpi))&mask)];
            }

        }
        
        
    }

    reset(clut:number[],pixels:number[],width:number,height:number):void{

        this.CLUT=clut;
        this.Indices=pixels;
        this.Width=width;
        this.Height=height;

        var bpi = get_bits_per_index(this.CLUT.length);
        var data_length_mult = (8/bpi);

        var mask = get_bit_mask(bpi);

        this.Data = new Uint16Array(this.Width * this.Height);
      
        for(var i =0; i<this.Indices.length;i++){

            var base_byte = this.Indices[i];

            for(var j = 0; j<data_length_mult; j++){

                
                this.Data[(i*data_length_mult)+j]=this.CLUT[((base_byte>>(j*bpi))&mask)];
            }

        }
        
        flip_tex_state();
    }

}

export class Audio{

    public Data:number[];
    public SampleRate :number;

    constructor(data:number[],sample_rate:number){

        this.Data=data;
        this.SampleRate=sample_rate;
    }

}