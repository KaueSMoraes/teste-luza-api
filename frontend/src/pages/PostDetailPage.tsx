import { useEffect, useState } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { getPost, deletePost, type Post } from '../api/posts';
import { useAuthStore } from '../store/authStore';

export default function PostDetailPage() {
  const { id } = useParams<{ id: string }>();
  const [post, setPost] = useState<Post | null>(null);
  const [loading, setLoading] = useState(true);
  const { user } = useAuthStore();
  const navigate = useNavigate();

  useEffect(() => {
    if (!id) return;
    getPost(id)
      .then(setPost)
      .finally(() => setLoading(false));
  }, [id]);

  const handleDelete = async () => {
    if (!post || !confirm('Tem certeza que deseja excluir esta postagem?')) return;
    await deletePost(post.id);
    navigate('/');
  };

  if (loading) return <p style={{ color: '#888', textAlign: 'center', marginTop: '4rem' }}>Carregando...</p>;
  if (!post) return <p style={{ color: '#e94560', textAlign: 'center', marginTop: '4rem' }}>Postagem não encontrada.</p>;

  const isAuthor = user && post.authorId === user.userId;

  return (
    <div style={styles.container}>
      <Link to="/" style={styles.back}>← Voltar</Link>
      <h1 style={styles.title}>{post.title}</h1>
      <p style={styles.meta}>
        Por <strong>{post.authorName}</strong> · {new Date(post.createdAt).toLocaleDateString('pt-BR', { year: 'numeric', month: 'long', day: 'numeric' })}
        {post.updatedAt && ` · Editado em ${new Date(post.updatedAt).toLocaleDateString('pt-BR')}`}
      </p>
      <div style={styles.content}>{post.content}</div>
      {isAuthor && (
        <div style={styles.actions}>
          <Link to={`/posts/${post.id}/edit`} style={styles.editBtn}>Editar</Link>
          <button onClick={handleDelete} style={styles.deleteBtn}>Excluir</button>
        </div>
      )}
    </div>
  );
}

const styles: Record<string, React.CSSProperties> = {
  container: { maxWidth: '800px', margin: '2rem auto', padding: '0 1rem' },
  back: { color: '#888', textDecoration: 'none', fontSize: '0.9rem' },
  title: { color: '#e94560', marginTop: '1rem', fontSize: '2rem' },
  meta: { color: '#888', fontSize: '0.9rem', marginBottom: '1.5rem' },
  content: { color: '#ddd', lineHeight: 1.8, fontSize: '1.05rem', whiteSpace: 'pre-wrap' },
  actions: { marginTop: '2rem', display: 'flex', gap: '1rem' },
  editBtn: { padding: '0.5rem 1.2rem', background: '#0f3460', color: '#fff', textDecoration: 'none', borderRadius: '4px', fontWeight: 500 },
  deleteBtn: { padding: '0.5rem 1.2rem', background: '#e94560', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer', fontWeight: 500 },
};
