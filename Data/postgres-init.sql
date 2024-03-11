BEGIN;

DO $$
DECLARE
    user_id UUID;
BEGIN
    -- Insere os registros, verificando se já existem
    FOR record IN
        SELECT * FROM (
            VALUES
                ('root', crypt('batatapalha' || gen_salt('vitor', 10)), 'email1@example.com', 'John', 'Doe', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                ('taobaixinho', crypt('batatapalha' || gen_salt('vitor', 10)), 'email2@example.com', 'Jane', 'Smith', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
        ) AS records("Username", "Password", "Email", "FirstName", "LastName", "CreatedAt", "VerifiedAt")
    LOOP
        BEGIN
            INSERT INTO public."Users"("Username", "Password", "Email", "FirstName", "LastName", "CreatedAt", "VerifiedAt")
            VALUES (record."Username", record."Password", record."Email", record."FirstName", record."LastName", record."CreatedAt", record."VerifiedAt")
            ON CONFLICT ("Username") DO NOTHING;
        END;
    END LOOP;
END $$;

COMMIT;