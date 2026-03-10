import { useEffect, useState } from 'react';
import { getPosts, type Post } from '../api/posts';
import PostList from '../components/Posts/PostList';
import { useSignalR } from '../hooks/useSignalR';

export default function HomePage() {
  const [posts, setPosts] = useState<Post[]>([]);
  const [notification, setNotification] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getPosts()
      .then(setPosts)
      .finally(() => setLoading(false));
  }, []);

  useSignalR((event) => {
    setNotification(`Nova postagem: "${event.title}"`);
    getPosts().then(setPosts);
    setTimeout(() => setNotification(null), 5000);
  });

  return (
    <div style={styles.container}>
      <h1 style={styles.heading}>Postagens</h1>
      {notification && <div style={styles.notification}>{notification}</div>}
      {loading ? <p style={{ color: '#888' }}>Carregando...</p> : <PostList posts={posts} />}
    </div>
  );
}

const styles: Record<string, React.CSSProperties> = {
  container: { maxWidth: '800px', margin: '2rem auto', padding: '0 1rem' },
  heading: { color: '#e94560', marginBottom: '1.5rem' },
  notification: {
    background: '#0f3460',
    border: '1px solid #e94560',
    borderRadius: '6px',
    padding: '0.75rem 1rem',
    marginBottom: '1rem',
    color: '#fff',
    fontSize: '0.95rem',
  },
};
