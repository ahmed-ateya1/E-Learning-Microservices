CREATE TABLE IF NOT EXISTS wishlists (
    wishlist_id UUID PRIMARY KEY,
    user_id UUID NOT NULL,
    course_id UUID NOT NULL,
    created_at TIMESTAMP NOT NULL,
    UNIQUE(user_id, course_id)
);

CREATE INDEX IF NOT EXISTS idx_wishlists_user_id ON wishlists(user_id);
CREATE INDEX IF NOT EXISTS idx_wishlists_course_id ON wishlists(course_id);