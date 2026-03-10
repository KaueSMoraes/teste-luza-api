import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { createPost } from '../api/posts';

export default function CreatePostPage() {
  const [title, setTitle] = useState('');
  const [content, setContent] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      const result = await createPost(title, content);
      navigate(`/posts/${result.id}`);
    } catch (err: unknown) {
      const msg = (err as { response?: { data?: { detail?: string } } })?.response?.data?.detail;
      setError(msg ?? 'Erro ao criar postagem.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={styles.container}>
      <h1 style={styles.title}>Nova Postagem</h1>
      {error && <p style={styles.error}>{error}</p>}
      <form onSubmit={handleSubmit}>
        <input style={styles.input} type="text" placeholder="Título" value={title} onChange={(e) => setTitle(e.target.value)} required maxLength={200} />
        <textarea style={styles.textarea} placeholder="Conteúdo" value={content} onChange={(e) => setContent(e.target.value)} required maxLength={10000} rows={12} />
        <button style={styles.button} type="submit" disabled={loading}>{loading ? 'Publicando...' : 'Publicar'}</button>
      </form>
    </div>
  );
}

const styles: Record<string, React.CSSProperties> = {
  container: { maxWidth: '800px', margin: '2rem auto', padding: '0 1rem' },
  title: { color: '#e94560', marginBottom: '1.5rem' },
  input: { display: 'block', width: '100%', padding: '0.75rem', marginBottom: '1rem', background: '#16213e', border: '1px solid #444', borderRadius: '4px', color: '#fff', fontSize: '1.1rem', boxSizing: 'border-box' },
  textarea: { display: 'block', width: '100%', padding: '0.75rem', marginBottom: '1rem', background: '#16213e', border: '1px solid #444', borderRadius: '4px', color: '#fff', fontSize: '1rem', resize: 'vertical', boxSizing: 'border-box' },
  button: { padding: '0.75rem 2rem', background: '#e94560', color: '#fff', border: 'none', borderRadius: '4px', fontSize: '1rem', cursor: 'pointer', fontWeight: 'bold' },
  error: { color: '#e94560', marginBottom: '1rem' },
};
