import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getPost, updatePost } from '../api/posts';
import { useAuthStore } from '../store/authStore';

export default function EditPostPage() {
  const { id } = useParams<{ id: string }>();
  const [title, setTitle] = useState('');
  const [content, setContent] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const { user } = useAuthStore();
  const navigate = useNavigate();

  useEffect(() => {
    if (!id) return;
    getPost(id).then((post) => {
      if (user && post.authorId !== user.userId) {
        navigate(`/posts/${id}`);
        return;
      }
      setTitle(post.title);
      setContent(post.content);
      setLoading(false);
    });
  }, [id]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!id) return;
    setError('');
    setSaving(true);
    try {
      await updatePost(id, title, content);
      navigate(`/posts/${id}`);
    } catch (err: unknown) {
      const msg = (err as { response?: { data?: { detail?: string } } })?.response?.data?.detail;
      setError(msg ?? 'Erro ao salvar postagem.');
    } finally {
      setSaving(false);
    }
  };

  if (loading) return <p style={{ color: '#888', textAlign: 'center', marginTop: '4rem' }}>Carregando...</p>;

  return (
    <div style={styles.container}>
      <h1 style={styles.title}>Editar Postagem</h1>
      {error && <p style={styles.error}>{error}</p>}
      <form onSubmit={handleSubmit}>
        <input style={styles.input} type="text" placeholder="Título" value={title} onChange={(e) => setTitle(e.target.value)} required maxLength={200} />
        <textarea style={styles.textarea} placeholder="Conteúdo" value={content} onChange={(e) => setContent(e.target.value)} required maxLength={10000} rows={12} />
        <button style={styles.button} type="submit" disabled={saving}>{saving ? 'Salvando...' : 'Salvar'}</button>
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
