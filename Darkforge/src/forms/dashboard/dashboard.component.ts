import { Component ,Input,AfterViewInit, OnDestroy, ViewChild, ElementRef} from '@angular/core';
import { AudioPlayerComponent } from '../../utils/controls/audio-player/audio-player.component';
import { FileTransferService } from '../../app/file_transfer/file_service/file-transfer.service';
import { Subscription } from 'rxjs';



@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [AudioPlayerComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  @Input() transfer!: FileTransferService;
  @ViewChild('wl_output',{static:false})wl_output? : AudioPlayerComponent;
  private taskCompletedSubscription!: Subscription;

  ngAfterViewInit(){
    this.taskCompletedSubscription = this.transfer.wlTaskCompleted$.subscribe(() => {
      
      this.wl_output?.set_audio();
      
    });
  }

  ngOnDestroy(){
    this.taskCompletedSubscription.unsubscribe();
  }

}
