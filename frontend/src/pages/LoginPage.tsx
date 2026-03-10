import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { login } from '../api/auth';
import { useAuthStore } from '../store/authStore';

export default function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const setAuth = useAuthStore((s) => s.setAuth);
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      const result = await login(email, password);
      setAuth(result.token, { userId: result.userId, email: result.email, userName: result.userName });
      navigate('/');
    } catch {
      setError('Email ou senha inválidos.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={styles.container}>
      <div style={styles.card}>
        <h1 style={styles.title}>Entrar</h1>
        {error && <p style={styles.error}>{error}</p>}
        <form onSubmit={handleSubmit}>
          <input style={styles.input} type="email" placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} required />
          <input style={styles.input} type="password" placeholder="Senha" value={password} onChange={(e) => setPassword(e.target.value)} required />
          <button style={styles.button} type="submit" disabled={loading}>{loading ? 'Entrando...' : 'Entrar'}</button>
        </form>
        <p style={styles.footer}>Não tem conta? <Link to="/register" style={styles.link}>Registre-se</Link></p>
      </div>
    </div>
  );
}

const styles: Record<string, React.CSSProperties> = {
  container: { display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '70vh' },
  card: { background: '#16213e', padding: '2rem', borderRadius: '8px', width: '100%', maxWidth: '400px' },
  title: { color: '#e94560', textAlign: 'center', marginBottom: '1.5rem' },
  input: { display: 'block', width: '100%', padding: '0.75rem', marginBottom: '1rem', background: '#0f3460', border: '1px solid #444', borderRadius: '4px', color: '#fff', fontSize: '1rem', boxSizing: 'border-box' },
  button: { width: '100%', padding: '0.75rem', background: '#e94560', color: '#fff', border: 'none', borderRadius: '4px', fontSize: '1rem', cursor: 'pointer', fontWeight: 'bold' },
  error: { color: '#e94560', marginBottom: '1rem', fontSize: '0.9rem' },
  footer: { textAlign: 'center', marginTop: '1rem', color: '#888', fontSize: '0.9rem' },
  link: { color: '#e94560' },
};
