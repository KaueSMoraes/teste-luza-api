import { Link, useNavigate } from 'react-router-dom';
import { useAuthStore } from '../../store/authStore';

export default function Navbar() {
  const { user, clearAuth } = useAuthStore();
  const navigate = useNavigate();

  const handleLogout = () => {
    clearAuth();
    navigate('/');
  };

  return (
    <nav style={styles.nav}>
      <Link to="/" style={styles.brand}>LuzaBlog</Link>
      <div style={styles.links}>
        {user ? (
          <>
            <span style={styles.username}>Olá, {user.userName}</span>
            <Link to="/posts/new" style={styles.link}>Nova Postagem</Link>
            <button onClick={handleLogout} style={styles.button}>Sair</button>
          </>
        ) : (
          <>
            <Link to="/login" style={styles.link}>Entrar</Link>
            <Link to="/register" style={styles.link}>Registrar</Link>
          </>
        )}
      </div>
    </nav>
  );
}

const styles: Record<string, React.CSSProperties> = {
  nav: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    padding: '1rem 2rem',
    background: '#1a1a2e',
    color: '#fff',
    boxShadow: '0 2px 4px rgba(0,0,0,0.3)',
  },
  brand: {
    fontSize: '1.5rem',
    fontWeight: 'bold',
    color: '#e94560',
    textDecoration: 'none',
  },
  links: { display: 'flex', gap: '1rem', alignItems: 'center' },
  link: { color: '#fff', textDecoration: 'none', fontWeight: 500 },
  username: { color: '#a0a0b0', fontSize: '0.9rem' },
  button: {
    background: 'transparent',
    border: '1px solid #e94560',
    color: '#e94560',
    padding: '0.3rem 0.8rem',
    borderRadius: '4px',
    cursor: 'pointer',
  },
};
