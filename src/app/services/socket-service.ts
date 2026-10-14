import { Injectable } from '@angular/core';
import { io, Socket } from 'socket.io-client';

// The only Socket.IO connection in the app. Being a root service, every
// component that injects it shares the same socket.
@Injectable({ providedIn: 'root' })
export class SocketService {
  private readonly socket: Socket = io('http://localhost:8080');

  on<T>(event: string, handler: (data: T) => void): void {
    this.socket.on(event, handler);
  }
}
