import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';

import { Harvest, NewHarvest } from '../models/harvest';
import { SocketService } from './socket-service';

@Injectable({ providedIn: 'root' })
export class HarvestService {
  private readonly http = inject(HttpClient);
  private readonly socket = inject(SocketService);
  private readonly apiUrl = 'http://localhost:8080/33603294/orchard/harvests';

  private readonly list = signal<Harvest[]>([]);
  readonly harvests = this.list.asReadonly();

  constructor() {
    // Registered once for the whole app, so changes made in another window
    // update the list straight from the event, without a new GET.
    this.socket.on<Harvest>('harvest:added', (harvest) => this.addToList(harvest));
    this.socket.on<{ id: string }>('harvest:removed', ({ id }) => this.removeFromList(id));
  }

  load(): Observable<Harvest[]> {
    return this.http.get<Harvest[]>(this.apiUrl).pipe(tap((harvests) => this.list.set(harvests)));
  }

  add(harvest: NewHarvest): Observable<Harvest> {
    return this.http.post<Harvest>(this.apiUrl, harvest).pipe(tap((created) => this.addToList(created)));
  }

  remove(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(tap(() => this.removeFromList(id)));
  }

  // The HTTP response and the socket event can both report the same record,
  // so a record already in the list is never added a second time.
  private addToList(harvest: Harvest): void {
    this.list.update((harvests) =>
      harvests.some((h) => h.id === harvest.id) ? harvests : [...harvests, harvest],
    );
  }

  private removeFromList(id: string): void {
    this.list.update((harvests) => harvests.filter((h) => h.id !== id));
  }
}
