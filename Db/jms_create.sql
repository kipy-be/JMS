-- -------------------------------------------------------------------------------------------------------------------------------------------------------
--    Tables
-- -------------------------------------------------------------------------------------------------------------------------------------------------------

-- Servers

    CREATE SEQUENCE s_servers;
    CREATE TABLE servers
    (
        server_id INTEGER NOT NULL DEFAULT NEXTVAL('s_servers'),
        server_name VARCHAR(128) NOT NULL,
        server_host VARCHAR(128) NOT NULL,
        server_port INTEGER NOT NULL,
        server_password VARCHAR(32) NULL,
        CONSTRAINT PK_server PRIMARY KEY (server_id)
    );
    ALTER SEQUENCE s_servers OWNED BY servers.server_id;
    
-- Chans

    CREATE SEQUENCE s_chans;
    CREATE TABLE chans
    (
        chan_id INTEGER NOT NULL DEFAULT NEXTVAL('s_chans'),
        server_id INTEGER NOT NULL,
        chan_name VARCHAR(128) NOT NULL,
        CONSTRAINT PK_chan PRIMARY KEY (chan_id)
    );
    ALTER SEQUENCE s_chans OWNED BY chans.chan_id;
    
-- Users

    CREATE SEQUENCE s_users;
    CREATE TABLE users
    (
        user_id INTEGER NOT NULL DEFAULT NEXTVAL('s_users'),
        server_id INTEGER NOT NULL,
        user_nick VARCHAR(24) NOT NULL,
        user_alternate_nick CHAR(24) NULL,
        CONSTRAINT PK_user PRIMARY KEY (user_id)
    );
    ALTER SEQUENCE s_users OWNED BY users.user_id;
    
-- Subscriptions

    CREATE TABLE subscriptions
    (
        user_id INTEGER NOT NULL,
        chan_id INTEGER NOT NULL,
        user_parent_id INTEGER NULL,
        subscription_registered_date TIMESTAMP NOT NULL,
        subscription_last_seen_date TIMESTAMP NULL,
        subscription_last_activity_date TIMESTAMP NULL,
        subscription_is_protected BOOL NOT NULL DEFAULT(false),
        subscription_can_add BOOL NOT NULL DEFAULT(false),
        subscription_can_delete BOOL NOT NULL DEFAULT(false),
        CONSTRAINT PK_subscription PRIMARY KEY (user_id, chan_id)
    );
    
-- Logs

    CREATE SEQUENCE s_logs;
    CREATE TABLE logs
    (
        log_id INTEGER NOT NULL DEFAULT NEXTVAL('s_logs'),
        log_from VARCHAR(24) NOT NULL,
        log_message VARCHAR(1024) NOT NULL,
        log_date TIMESTAMP NOT NULL,
        CONSTRAINT PK_log PRIMARY KEY (log_id)
    );
    ALTER SEQUENCE s_logs OWNED BY logs.log_id;
    
-- -------------------------------------------------------------------------------------------------------------------------------------------------------
--    FK / Indexes / Contsraints
-- -------------------------------------------------------------------------------------------------------------------------------------------------------


-- Chans

    -- relation chan <-> server
    ALTER TABLE chans ADD CONSTRAINT FK_chans__server_id
        FOREIGN KEY (server_id) REFERENCES servers (server_id);
        
    CREATE INDEX IX_chans__server_id
        ON chans (server_id);
        
-- Users

    -- relation user <-> server
    ALTER TABLE users ADD CONSTRAINT FK_users__server_id
        FOREIGN KEY (server_id) REFERENCES servers (server_id);
        
    CREATE INDEX IX_users__server_id
        ON users (server_id);
        
    -- Unicity
    CREATE UNIQUE INDEX IX_UQ_users__server_id__user_nick
        ON users (server_id, user_nick);
        
-- Subscriptions

    -- relation subscription <-> user
    ALTER TABLE subscriptions ADD CONSTRAINT FK_subscriptions__user_id
        FOREIGN KEY (user_id) REFERENCES users (user_id);
        
    CREATE INDEX IX_subscriptions__user_id
        ON subscriptions (user_id);
        
    -- relation subscription <-> user 2
    ALTER TABLE subscriptions ADD CONSTRAINT FK_subscriptions__user_parent_id
        FOREIGN KEY (user_parent_id) REFERENCES users (user_id);
        
    CREATE INDEX IX_subscriptions__user_parent_id
        ON subscriptions (user_parent_id);
        
    -- relation subscription <-> chan
    ALTER TABLE subscriptions ADD CONSTRAINT FK_subscriptions__chan_id
        FOREIGN KEY (chan_id) REFERENCES chans (chan_id);
        
    CREATE INDEX IX_subscriptions__chan_id
        ON subscriptions (chan_id);