import { DatePipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import { ConfirmDialog33603294 } from '../confirm-dialog-33603294/confirm-dialog-33603294';
import { Harvest } from '../models/harvest';
import { Shorten33603294Pipe } from '../pipes/shorten-33603294-pipe';
import { HarvestService } from '../services/harvest-service';

@Component({
  selector: 'app-harvest-list-33603294',
  imports: [RouterLink, DatePipe, Shorten33603294Pipe, ConfirmDialog33603294],
  templateUrl: './harvest-list-33603294.html',
  styleUrl: './harvest-list-33603294.css',
})
export class HarvestList33603294 implements OnInit {
  private readonly harvestService = inject(HarvestService);

  protected readonly harvests = this.harvestService.harvests;
  protected readonly pendingDelete = signal<Harvest | null>(null);
  protected readonly errorMessage = signal('');

  ngOnInit(): void {
    this.harvestService.load().subscribe({
      error: () => this.errorMessage.set('Could not load the harvests. Is the API running on port 8080?'),
    });
  }

  askToDelete(harvest: Harvest): void {
    this.errorMessage.set('');
    this.pendingDelete.set(harvest);
  }

  cancelDelete(): void {
    this.pendingDelete.set(null);
  }

  confirmDelete(harvest: Harvest): void {
    this.pendingDelete.set(null);
    this.harvestService.remove(harvest.id).subscribe({
      error: () => this.errorMessage.set(`Could not delete ${harvest.variety}.`),
    });
  }
}
