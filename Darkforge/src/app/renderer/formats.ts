import { get_bit_mask, get_bits_per_index,get_pcm_value } from "../../utils/bitfield_helper";
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

        let  bpi = get_bits_per_index(this.CLUT.length);
        let data_length_mult = (8/bpi);

        let mask = get_bit_mask(bpi);

        this.Data = new Uint16Array(this.Width * this.Height);
      
        for(let i =0; i<this.Indices.length;i++){

            let base_byte = this.Indices[i];

            for(let j = data_length_mult-1; j>=0; j--){
                
                this.Data[((i*data_length_mult)+(data_length_mult-1-j))%(this.Width*this.Height)]=this.CLUT[((base_byte>>(j*bpi))&mask)];
                              
            }


        }
        
       this.CLUT=[];
       this.Indices=[];
        
    }

    public reset(clut:number[],pixels:number[],width:number,height:number):void{

        this.CLUT=clut;
        this.Indices=pixels;
        this.Width=width;
        this.Height=height;

        let bpi = get_bits_per_index(this.CLUT.length);
        let data_length_mult = (8/bpi);

        let mask = get_bit_mask(bpi);

        this.Data = new Uint16Array(this.Width * this.Height);
      
        for(let i =0; i<this.Indices.length;i++){

            let base_byte = this.Indices[i];

            for(let j = data_length_mult-1; j>=0; j--){
                
                this.Data[((i*data_length_mult)+(data_length_mult-1-j))%(this.Width*this.Height)]=this.CLUT[((base_byte>>(j*bpi))&mask)];
                              
            }


        }
        
        this.CLUT=[];
        this.Indices=[];

        flip_tex_state();
    }

}

export class Audio{

    public BlockData: number[];
    public Data:Float32Array;
    public SampleRate :number;
    public ChannelCount : number;
    public BlocksPerChannel:number;
    public Looping :boolean;
    public ThresholdBits:number;
    public Samples : number[];

    constructor(data:number[],sample_rate:number,channels:number,blocks:number,threshold:number){

       
        this.BlockData=data;
        this.SampleRate=sample_rate;
        this.ChannelCount=channels;
        this.BlocksPerChannel=blocks;
        
        this.ThresholdBits=threshold;
        this.Looping= this.BlockData[2]==6;
        this.Samples = [];


       

        for(let i = 0; i < (this.BlocksPerChannel*this.ChannelCount*16);i+=16){

            let total_shift = ((this.BlockData[i]>>4)&0xF) + this.ThresholdBits;
          

            for(let j = 0; j<14;j++){

                let samp = this.BlockData[(i+2+j)];

                this.Samples.push(get_pcm_value(((samp>>4)&0xF),total_shift));
               

                this.Samples.push(get_pcm_value((samp&0xF),total_shift));
               
              
            }


        }        


       

        for(let i = 0; i < this.Samples.length;i++){
             
            if(i>=(this.ChannelCount*3)){

                this.Samples[(i-(2*this.ChannelCount))]+=((this.Samples[i]-this.Samples[(i-(3*this.ChannelCount))])*0.25);
                this.Samples[(i-this.ChannelCount)]+=((this.Samples[i]-this.Samples[(i-(3*this.ChannelCount))])*0.75);

            }

            
        }

        this.BlockData=[];

        this.Data = new Float32Array(this.Samples);

        this.Samples=[];

    }


    public reset(data:number[],sample_rate:number,channels:number,blocks:number,threshold:number):void{

        this.BlockData=data;
        this.SampleRate=sample_rate;
        this.ChannelCount=channels;
        this.BlocksPerChannel=blocks;
        
        this.ThresholdBits=threshold;
        this.Looping= this.BlockData[1]==6;
        this.Samples = [];


        for(let i = 0; i < (this.BlocksPerChannel*this.ChannelCount*16);i+=16){

            let total_shift = ((this.BlockData[i]>>4)&0xF) + this.ThresholdBits;
          

            for(let j = 0; j<14;j++){

                let samp = this.BlockData[(i+2+j)];

                this.Samples.push(get_pcm_value(((samp>>4)&0xF),total_shift));
               

                this.Samples.push(get_pcm_value((samp&0xF),total_shift));
               
              
            }


        }        


       

        for(let i = 0; i < this.Samples.length;i++){
             
            if(i>=(this.ChannelCount*3)){

                this.Samples[(i-(2*this.ChannelCount))]+=((this.Samples[i]-this.Samples[(i-(3*this.ChannelCount))])*0.25);
                this.Samples[(i-this.ChannelCount)]+=((this.Samples[i]-this.Samples[(i-(3*this.ChannelCount))])*0.75);

            }

            
        }

        this.BlockData=[];

        this.Data = new Float32Array(this.Samples);

        this.Samples=[];

    }

}