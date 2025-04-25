import Link from "next/link";

import { Footer } from "@/components/footer";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { LogOutForm } from "@/features/auth/logout-form";
import { routes } from "@/lib/routes";

export default function SignOut() {
  return (
    <div className="grid min-h-dvh grid-rows-[auto_1fr_auto] gap-16">
      <header className="bg-background/60 sticky top-0 z-50 py-4 backdrop-blur-sm">
        <div className="container mx-auto">
          <Link href={routes.chats} className="font-medium">
            ChatAnalyzer
          </Link>
        </div>
      </header>

      <main className="container mx-auto max-w-xl">
        <Card>
          <CardHeader>
            <CardTitle>Log Out</CardTitle>
            <CardDescription>
              Are you sure you want to log out? This will end your current
              session.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <LogOutForm />
          </CardContent>
        </Card>
      </main>

      <Footer />
    </div>
  );
}
