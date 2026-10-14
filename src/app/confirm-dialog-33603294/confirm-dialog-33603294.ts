import { AfterViewInit, Component, ElementRef, input, output, viewChild } from '@angular/core';

@Component({
  selector: 'app-confirm-dialog-33603294',
  templateUrl: './confirm-dialog-33603294.html',
  styleUrl: './confirm-dialog-33603294.css',
})
export class ConfirmDialog33603294 implements AfterViewInit {
  readonly message = input.required<string>();
  readonly confirmed = output<void>();
  readonly cancelled = output<void>();

  private readonly dialog = viewChild.required<ElementRef<HTMLDialogElement>>('dialog');

  ngAfterViewInit(): void {
    // A modal <dialog> makes the rest of the page inert until it closes,
    // and pressing Escape fires its "cancel" event.
    this.dialog().nativeElement.showModal();
  }
}
