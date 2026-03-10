import { useEffect } from 'react';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

export interface NewPostEvent {
  postId: string;
  title: string;
  authorId: string;
}

export function useSignalR(onPostPublished: (event: NewPostEvent) => void) {
  useEffect(() => {
    const connection = new HubConnectionBuilder()
      .withUrl(import.meta.env.VITE_HUB_URL)
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Warning)
      .build();

    connection.on('PostPublished', onPostPublished);

    connection.start().catch(() => {});

    return () => {
      connection.stop();
    };
  }, []);
}
