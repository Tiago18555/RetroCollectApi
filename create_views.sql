DO $$ 
BEGIN
    IF NOT EXISTS (SELECT FROM pg_views WHERE viewname = 'user_view') THEN
        -- except password
        CREATE VIEW user_view AS
        SELECT 
            "user_id",
            "username",
            "email",
            "verified_at",
            "first_name",
            "last_name",
            "created_at",
            "updated_at"
        FROM "user";
    END IF;
    IF NOT EXISTS (SELECT FROM pg_views WHERE viewname = 'user_view_clean') THEN
        -- except timestamp / binary type, password
        CREATE VIEW user_view_clean AS
        SELECT 
            "username",
            "email",
            "first_name",
            "last_name"
        FROM "user";
    END IF;
END $$;
