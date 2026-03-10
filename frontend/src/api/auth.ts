import client from './client';

export interface AuthResult {
  token: string;
  userId: string;
  email: string;
  userName: string;
}

export const register = (email: string, password: string, userName: string) =>
  client.post<AuthResult>('/api/auth/register', { email, password, userName }).then((r) => r.data);

export const login = (email: string, password: string) =>
  client.post<AuthResult>('/api/auth/login', { email, password }).then((r) => r.data);
