import { Component ,Input,AfterViewInit, OnDestroy, OnInit,ViewChild, ElementRef} from '@angular/core';
import { AudioPlayerComponent } from '../../utils/controls/audio-player/audio-player.component';
import { FileTransferService } from '../../app/file_transfer/file_service/file-transfer.service';
import { Subscription } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { update_anim, ast, current_anim$} from '../../assets/global_assets';
import { Asset } from '../../app/renderer/formats';
import { TranslateService,TranslatePipe, TranslateDirective} from '@ngx-translate/core';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [AudioPlayerComponent, FormsModule,CommonModule,TranslatePipe],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  @Input() transfer!: FileTransferService;
  @ViewChild('wl_output',{static:false})wl_output? : AudioPlayerComponent;
  private taskCompletedSubscription!: Subscription;
  private animSubscription!: Subscription;
  public anim:number|null=null;
  public ast_ref:Asset;


  constructor(public translate:TranslateService) {
    this.ast_ref=ast;
    
  }

  public update($event:number|null):void{
    
    update_anim($event);

  }


  ngOnInit(){

    this.animSubscription=current_anim$.subscribe($value=>{

      this.anim=$value;
    });


    this.taskCompletedSubscription = this.transfer.wlTaskCompleted$.subscribe(() => {
      
      this.wl_output?.set_audio();
      
    });
  }

  ngOnDestroy(){
    this.taskCompletedSubscription.unsubscribe();
    this.animSubscription.unsubscribe();
  }

}
