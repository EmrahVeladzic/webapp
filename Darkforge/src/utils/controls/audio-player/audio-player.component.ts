import { Component ,ViewChild,  AfterViewInit,  OnDestroy, ElementRef, Input } from '@angular/core';
import { SliderComponent } from "../slider/slider.component";
import {Subscription } from 'rxjs';
import { Audio } from '../../../app/renderer/formats';
import { sfx } from '../../../assets/global_assets';
import { PropertyWrite } from '@angular/compiler';
import { TranslateService,TranslatePipe, TranslateDirective} from '@ngx-translate/core';


@Component({
  selector: 'app-audio-player',
  standalone: true,
  imports: [SliderComponent, TranslatePipe],
  templateUrl: './audio-player.component.html',
  styleUrl: './audio-player.component.css'
})
export class AudioPlayerComponent {
  @ViewChild('volume',{static:false})volume!:SliderComponent;
  @ViewChild('audio',{static:false})audio!:ElementRef<HTMLAudioElement>;
  private volumeSubscription!: Subscription;

  private audioContext? : AudioContext;
  private gain? : GainNode;

  private source? : AudioBufferSourceNode = undefined;
  
  private playing : boolean = false;

  @Input() standard_format : boolean = true;

  private buffer?: AudioBuffer;

  constructor(public translate : TranslateService){

  }

  ngAfterViewInit(){

    if(this.standard_format){
    
      this.volumeSubscription=this.volume.valueChanges$.subscribe($value=>{      


        this.audio.nativeElement.volume = this.volume.getValue()/100;
      
    
      });

      this.audio.nativeElement.loop=true;
    }
 
    else{
      this.set_audio();
    }
  }

  public set_audio():void{

    if(this.source && this.playing){

      this.playing=false;
      this.source.stop();
      

    }



    if(!this.audioContext && ! this.gain){

      this.audioContext =  new window.AudioContext();

      this.gain = this.audioContext.createGain();

      this.volumeSubscription=this.volume.valueChanges$.subscribe($value=>{      
        
        this.gain!.gain.value = $value/((sfx.ThresholdBits/4)*(Math.pow(2,sfx.ThresholdBits)));
    
      });

    }

    this.gain!.gain.value = this.volume.getValue()/((sfx.ThresholdBits/4)*(Math.pow(2,sfx.ThresholdBits)));
 

    this.buffer = this.audioContext!.createBuffer(sfx.ChannelCount,(sfx.BlocksPerChannel*28),sfx.SampleRate)

    let channel_data : number[] [] = [];

    for(let i = 0; i < sfx.ChannelCount; i++){
      channel_data.push([]);
    }

    

    for (let i = 0; i < sfx.Data.length; i++) {
     
      channel_data[i%sfx.ChannelCount].push(sfx.Data[i]);
    
    
      
    }
   

    for(let i = 0; i < sfx.ChannelCount; i++){

      this.buffer.getChannelData(i).set(new Float32Array(channel_data[i]));

      channel_data[i]=[];
    }

    channel_data=[];


  }

  playButtonEvent($event:Event):void{

    if(this.standard_format){
      if(this.audio.nativeElement.currentTime>0&&!this.audio.nativeElement.paused){
        this.audio.nativeElement.pause();
      }
      else{
        this.audio.nativeElement.currentTime = 0;
        this.audio.nativeElement.play();
      }
    }

    else{     
      
        
      if(!this.audioContext){
        
        this.set_audio();

      

      }

      else if(this.playing){
        this.playing=false;
        this.source?.stop();     
      }
      else{

        
        this.source = this.audioContext!.createBufferSource();  
        
       

        this.source.buffer = this.buffer!;

        this.source.loop=sfx.Looping;

        this.source.loopStart = 0;
    
        this.source.loopEnd = this.buffer!.duration;

                     
        this.source.connect(this.gain!);
           

        this.gain!.connect(this.audioContext!.destination);

        this.source?.start(0);
        this.playing=true;



        this.source.onended = () => {
                       
          if(this.source?.loop && this.playing){

           
            this.source = this.audioContext!.createBufferSource();
    
            this.source.buffer = this.buffer!;

                         
            this.source.connect(this.gain!);
               
    
            this.gain!.connect(this.audioContext!.destination);
    
            this.source?.start(0);
             

          }

          else if(!this.source?.loop && this.playing){

            this.playing=false;
          }

         

        };

      }

      
    }

  }

  ngOnDestroy(){
    this.volumeSubscription.unsubscribe();
  }




}
