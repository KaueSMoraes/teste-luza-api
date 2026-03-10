import { Link } from 'react-router-dom';
import type { Post } from '../../api/posts';

export default function PostCard({ post }: { post: Post }) {
  const excerpt = post.content.length > 150 ? post.content.slice(0, 150) + '...' : post.content;

  return (
    <div style={styles.card}>
      <h2 style={styles.title}>
        <Link to={`/posts/${post.id}`} style={styles.link}>{post.title}</Link>
      </h2>
      <p style={styles.meta}>
        Por <strong>{post.authorName}</strong> · {new Date(post.createdAt).toLocaleDateString('pt-BR')}
        {post.updatedAt && ' · Editado'}
      </p>
      <p style={styles.excerpt}>{excerpt}</p>
      <Link to={`/posts/${post.id}`} style={styles.readMore}>Ler mais →</Link>
    </div>
  );
}

const styles: Record<string, React.CSSProperties> = {
  card: {
    background: '#16213e',
    borderRadius: '8px',
    padding: '1.5rem',
    marginBottom: '1rem',
    boxShadow: '0 2px 8px rgba(0,0,0,0.2)',
  },
  title: { margin: '0 0 0.5rem', fontSize: '1.3rem' },
  link: { color: '#e94560', textDecoration: 'none' },
  meta: { color: '#888', fontSize: '0.85rem', margin: '0 0 0.75rem' },
  excerpt: { color: '#ccc', lineHeight: 1.6, margin: '0 0 1rem' },
  readMore: { color: '#e94560', textDecoration: 'none', fontWeight: 500, fontSize: '0.9rem' },
};
