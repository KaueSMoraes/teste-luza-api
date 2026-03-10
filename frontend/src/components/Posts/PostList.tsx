import type { Post } from '../../api/posts';
import PostCard from './PostCard';

export default function PostList({ posts }: { posts: Post[] }) {
  if (posts.length === 0) {
    return <p style={{ color: '#888', textAlign: 'center' }}>Nenhuma postagem ainda. Seja o primeiro!</p>;
  }
  return (
    <div>
      {posts.map((p) => (
        <PostCard key={p.id} post={p} />
      ))}
    </div>
  );
}
