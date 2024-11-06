import { Component ,ViewChild,  AfterViewInit,  OnDestroy, ElementRef } from '@angular/core';
import { SliderComponent } from "../slider/slider.component";
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-audio-player',
  standalone: true,
  imports: [SliderComponent],
  templateUrl: './audio-player.component.html',
  styleUrl: './audio-player.component.css'
})
export class AudioPlayerComponent {
  @ViewChild('volume',{static:false})volume!:SliderComponent;
  @ViewChild('audio',{static:false})audio!:ElementRef<HTMLAudioElement>;
  private volumeSubscription!: Subscription;


  ngAfterViewInit(){
    this.volumeSubscription=this.volume.valueChanges$.subscribe($value=>{      
      this.audio.nativeElement.volume = this.volume.getValue()/100;
      this.audio.nativeElement.loop=true;
    });
  }

  playButtonEvent($event:Event):void{
    if(this.audio.nativeElement.currentTime>0&&!this.audio.nativeElement.paused){
      this.audio.nativeElement.pause();
    }
    else{
      this.audio.nativeElement.currentTime = 0;
      this.audio.nativeElement.play();
    }
  }

  ngOnDestroy(){
    this.volumeSubscription.unsubscribe();
  }




}
