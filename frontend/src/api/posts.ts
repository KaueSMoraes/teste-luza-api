import client from './client';

export interface Post {
  id: string;
  title: string;
  content: string;
  authorId: string;
  authorName: string;
  createdAt: string;
  updatedAt: string | null;
}

export const getPosts = () =>
  client.get<Post[]>('/api/posts').then((r) => r.data);

export const getPost = (id: string) =>
  client.get<Post>(`/api/posts/${id}`).then((r) => r.data);

export const createPost = (title: string, content: string) =>
  client.post<{ id: string }>('/api/posts', { title, content }).then((r) => r.data);

export const updatePost = (id: string, title: string, content: string) =>
  client.put(`/api/posts/${id}`, { title, content });

export const deletePost = (id: string) =>
  client.delete(`/api/posts/${id}`);
